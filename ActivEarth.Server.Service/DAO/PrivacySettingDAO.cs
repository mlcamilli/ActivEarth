using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class PrivacySettingDAO
    {
        /// <summary>
        /// Retrieves a User's PrivacySetting from the DB based on the user's ID.
        /// </summary>
        /// <param name="privacySettingId">Identifier of the user to retrieve the privacy setting for.</param>
        /// <returns>Privacy setting specified by the provided user ID.</returns>
        public static PrivacySetting GetPrivacySettingFromUserId(int userId)
        {
            PrivacySetting toReturn;
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from p in data.PrivacySettingDataProviders
                            where p.user_id == userId
                            select
                                new PrivacySetting
                                {
                                    ID = p.id,
                                    Email = p.email,
                                    Gender = p.gender,
                                    Age = p.age,
                                    Height = p.height,
                                    Weight = p.weight,
                                    Group = p.groups,
                                    ProfileVisibility = (ProfileVisibility)p.profile_visibility,
                                    UserID = p.user_id
                                }).FirstOrDefault();
            }

            if (toReturn != null)
            {
                toReturn.User = UserDAO.GetUserFromUserId(toReturn.UserID);
            }
            return toReturn;
        }

        /// <summary>
        /// Retrieves a PrivacySetting from the DB based on its ID.
        /// </summary>
        /// <param name="privacySettingId">Identifier of the privacy setting to retrieve.</param>
        /// <returns>Privacy setting specified by the provided ID.</returns>
        public static PrivacySetting GetPrivacySettingFromPrivacySettingId(int privacySettingId)
        {
            PrivacySetting toReturn;
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from p in data.PrivacySettingDataProviders
                        where p.id == privacySettingId
                        select
                            new PrivacySetting
                            {
                                ID = p.id,
                                Email = p.email,
                                Gender = p.gender,
                                Age = p.age,
                                Height = p.height,
                                Weight = p.weight,
                                Group = p.groups,
                                ProfileVisibility = (ProfileVisibility)p.profile_visibility,
                                UserID = p.user_id
                            }).FirstOrDefault();
            }

            if (toReturn != null)
            {
                toReturn.User = UserDAO.GetUserFromUserId(toReturn.UserID);
            }
            return toReturn;
        }

        /// <summary>
        /// Saves a privacy setting as a new entry in the DB.
        /// </summary>
        /// <param name="privacySetting">Privacy setting object to add to the DB.</param>
        /// <returns>ID of the created privacy setting on success, 0 on failure.</returns>
        public static int CreateNewPrivacySetting(PrivacySetting privacySetting)
        {
            try
            {
                int id;

                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var privacySettingData = new PrivacySettingDataProvider
                    {
                        id = privacySetting.ID,
                        email = privacySetting.Email,
                        gender = privacySetting.Gender,
                        age = privacySetting.Age,
                        height = privacySetting.Height,
                        weight = privacySetting.Weight,
                        profile_visibility = (Byte)privacySetting.ProfileVisibility,
                        user_id = privacySetting.UserID
                    };

                    data.PrivacySettingDataProviders.InsertOnSubmit(privacySettingData);
                    data.SubmitChanges();

                    id = privacySettingData.id;
                }

                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing PrivacySetting in the DB.
        /// </summary>
        /// <param name="privacySetting">PrivacySetting whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdatePrivacySetting(PrivacySetting privacySetting)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    PrivacySettingDataProvider dbPrivacySetting =
                        (from p in data.PrivacySettingDataProviders where p.id == privacySetting.ID select p).FirstOrDefault();

                    if (dbPrivacySetting != null)
                    {
                        dbPrivacySetting.email = privacySetting.Email;
                        dbPrivacySetting.gender = privacySetting.Gender;
                        dbPrivacySetting.age = privacySetting.Age;
                        dbPrivacySetting.height = privacySetting.Height;
                        dbPrivacySetting.weight = privacySetting.Weight;
                        dbPrivacySetting.profile_visibility = (Byte)privacySetting.ProfileVisibility;

                        data.SubmitChanges();
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an existing PrivacySetting from the DB.
        /// </summary>
        /// <param name="privacySettingId">ID for the PrivacySetting whose record needs to be removed.</param>
        /// <returns>True on success (or the privacy setting didn't exist), false on failure.</returns>
        public static bool RemovePrivacySettingFromPrivacySettingId(int privacySettingId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    PrivacySettingDataProvider dbPrivacySetting =
                        (from p in data.PrivacySettingDataProviders where p.id == privacySettingId select p).FirstOrDefault();

                    if (dbPrivacySetting != null)
                    {
                        data.PrivacySettingDataProviders.DeleteOnSubmit(dbPrivacySetting);
                        data.SubmitChanges();
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}