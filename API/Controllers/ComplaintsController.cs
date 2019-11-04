﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using ModelsLibrary.Data;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ComplaintsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Gets all complaints
        /// </summary>
        /// <returns>All complaints</returns>
        /// <response code="200">Returns All Complaints</response>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Complaint>>> GetComplaints()
        {
            return await _context.Complaints.ToListAsync()
                                            .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets Specified Complaint
        /// </summary>
        /// <param name="placeId">Place Id of complaint's place</param>
        /// <param name="userId">User Id of complaint's user</param>
        /// <param name="timeSubmitted">Time when compaint submitted</param>
        /// <returns>Requested Complaint</returns>
        /// <response code="200">Returns requested complaint</response>
        /// <response code="404">No complaint Found</response>
        [HttpGet("{placeId}/{userId}/{timeSubmitted}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Complaint>> GetComplaint(string placeId, string userId, DateTime timeSubmitted)
        {
            Complaint complaint = await _context.Complaints.FindAsync(placeId, userId, timeSubmitted);

            if (complaint == null)
            {
                return NotFound();
            }

            return complaint;
        }

        /// <summary>
        /// Posts new complaint
        /// </summary>
        /// <param name="newComplaint">Complaint to add</param>
        /// <returns>Added Complaint</returns>
        /// <response code="201">Returns Posted Review</response>
        /// <response code="400">Review is undefined</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NewComplaint>> PostComplaint(NewComplaint newComplaint)
        {
            if (newComplaint == null)
            {
                return BadRequest();
            }

            if (!HasOwnedDataAccess(newComplaint.UserID))
            {
                return Forbid();
            }

            _context.Complaints.Add(new Complaint 
            { 
                PlaceID = newComplaint.PlaceID, 
                UserID = newComplaint.UserID, 
                Comment = newComplaint.Comment 
            });

            DateTime timeBeforeSave = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync()
                              .ConfigureAwait(false);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            Complaint createdComplaint = await _context.Complaints.FirstAsync(c =>
                                                                   c.UserID == newComplaint.UserID &&
                                                                   c.PlaceID == newComplaint.PlaceID &&
                                                                   c.TimeSubmitted > timeBeforeSave)
                                                                  .ConfigureAwait(false);

            return CreatedAtAction("GetComplaint", new { 
                placeId = createdComplaint.PlaceID, 
                userId = createdComplaint.UserID, 
                timeSubmitted = createdComplaint.TimeSubmitted 
            }, 
            createdComplaint);
    }

        /// <summary>
        /// Puts updated complaint
        /// </summary>
        /// <param name="placeId">Complaints place id</param>
        /// <param name="userId">Complaints user id</param>
        /// <param name="timeSubmitted">Time complaint was submitted</param>
        /// <param name="complaint">Updated complaint</param>
        /// <returns>Action result</returns>
        /// <response code="204">Complaint Successfully Updated</response>
        /// <response code="400">Invalid Input</response>
        /// <response code="404">Complaint Doesn't Exist</response>
        [HttpPut("{placeId}/{userId}/{timeSubmitted}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutComplaint(string placeId, string userId, DateTime timeSubmitted, Complaint complaint)
        {
            if (complaint == null || placeId != complaint.PlaceID || userId != complaint.UserID || timeSubmitted != complaint.TimeSubmitted)
            {
                return BadRequest();
            }

            _context.Entry(complaint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync()
                              .ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ComplaintExists(placeId, userId, timeSubmitted).ConfigureAwait(false))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Delete specified Complaint
        /// </summary>
        /// <param name="placeId">Place Id for Complaint</param>
        /// <param name="userId">User Id for Complaint</param>
        /// <param name="timeSubmitted">Time Complaint Submitted</param>
        /// <returns>Deleted Complaint</returns>
        /// <response code="200">Returns Deleted Complaint</response>
        /// <response code="404">Complaint Not Found</response>
        [HttpDelete("{placeId}/{userId}/{timeSubmitted}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Complaint>> DeleteComplaint(string placeId, string userId, DateTime timeSubmitted)
        {
            var complaint = await _context.Complaints.FindAsync(placeId, userId, timeSubmitted);
            if (complaint == null)
            {
                return NotFound();
            }

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync()
                          .ConfigureAwait(false);

            return complaint;
        }

        private async Task<bool> ComplaintExists(string placeId, string userId, DateTime timeSubmitted)
        {
            return await _context.Complaints.AnyAsync(e => e.PlaceID == placeId && e.UserID == userId && e.TimeSubmitted == timeSubmitted)
                                            .ConfigureAwait(false);
        }

        private bool HasOwnedDataAccess(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim.Value == userId)
            {
                return true;
            }
            else
            {
                var roleClaims = claimsIdentity.FindAll(ClaimTypes.Role);
                if (roleClaims.Any(claim => claim.Value == "Administrator"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}