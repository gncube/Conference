﻿using Connect.Conference.Core.Common;
using Connect.Conference.Core.Repositories;
using DotNetNuke.Entities.Users;

namespace Connect.Conference.Core.Controllers
{
    public class ConferenceController
    {
        public static int AddAttendee(int portalId, int conferenceId, int userId, string email, string firstName, string lastName, string displayName, string company, int updatingUserId)
        {
            var user = UserController.Instance.GetUserById(portalId, userId);
            if (user == null)
            {
                user = UserController.GetUserByEmail(portalId, email);
                if (user == null)
                {
                    user = new UserInfo() { PortalID = portalId, Username = email, Email = email, FirstName = firstName, LastName = lastName, DisplayName = displayName };
                    user.Membership.Password = UserController.GeneratePassword();
                    user.Membership.Approved = true;
                    user.Membership.UpdatePassword = true;
                    var res = UserController.CreateUser(ref user);
                    if (res != DotNetNuke.Security.Membership.UserCreateStatus.Success)
                    {
                        throw new System.Exception(res.ToString());
                    }
                }
            }
            var attendee = AttendeeRepository.Instance.GetAttendee(conferenceId, user.UserID);
            if (attendee == null)
            {
                attendee = new Models.Attendees.Attendee() { ConferenceId = conferenceId, UserId = user.UserID, Status = (int)AttendeeStatus.Confirmed, ReceiveNotifications = true, Company = company };
                AttendeeRepository.Instance.AddAttendee(attendee, updatingUserId);
            }
            else
            {
                if (!string.IsNullOrEmpty(company))
                {
                    attendee.Company = company;
                }
                attendee.Status = (int)AttendeeStatus.Confirmed;
                AttendeeRepository.Instance.UpdateAttendee(attendee, updatingUserId);
            }
            DnnRoleController.CheckAttendee(portalId, conferenceId, user.UserID);
            return user.UserID;
        }

        public static int AddSpeaker(int portalId, int conferenceId, int userId, string email, string firstName, string lastName, string displayName, string company, int updatingUserId)
        {
            var user = UserController.Instance.GetUserById(portalId, userId);
            if (user == null)
            {
                user = UserController.GetUserByEmail(portalId, email);
                if (user == null)
                {
                    user = new UserInfo() { PortalID = portalId, Username = email, Email = email, FirstName = firstName, LastName = lastName, DisplayName = displayName };
                    user.Membership.Password = UserController.GeneratePassword();
                    user.Membership.Approved = true;
                    user.Membership.UpdatePassword = true;
                    var res = UserController.CreateUser(ref user);
                    if (res != DotNetNuke.Security.Membership.UserCreateStatus.Success)
                    {
                        throw new System.Exception(res.ToString());
                    }
                }
            }
            var speaker = SpeakerRepository.Instance.GetSpeaker(conferenceId, user.UserID);
            if (speaker == null)
            {
                var s = new Models.Speakers.SpeakerBase() { ConferenceId = conferenceId, UserId = user.UserID, Company = company };
                SpeakerRepository.Instance.AddSpeaker(s, updatingUserId);
            }
            DnnRoleController.CheckSpeaker(portalId, conferenceId, user.UserID);
            return user.UserID;
        }
    }
}
