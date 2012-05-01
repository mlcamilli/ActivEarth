using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Account
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["userDetails"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var userDetails = (User) Session["userDetails"];
                    lblUserName.Text = userDetails.UserName;
                    tbFirstName.Text = userDetails.FirstName;
                    tbLastName.Text = userDetails.LastName;
                    tbEmail.Text = userDetails.Email;
                    ddlGender.SelectedValue = userDetails.Gender + "";
                    tbCity.Text = userDetails.City;
                    tbState.Text = userDetails.State;
                    tbAge.Text = userDetails.Age.ToString();
                    tbHeight.Text = userDetails.Height.ToString();
                    tbWeight.Text = userDetails.Weight.ToString();

                }
            }
        }

        protected void CancelSaveUserProfile(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Profile.aspx");
        }
        protected void SaveUserProfile(object sender, EventArgs e)
        {

            if (tbAge.Text == "")
            {
                tbAge.Text = "0";
            }
            if (tbHeight.Text == "")
            {
                tbHeight.Text = "0";
            }
            if (tbWeight.Text == "")
            {
                tbWeight.Text = "0";
            }

            var user = new User
                           {
                               UserID = ((User)Session["userDetails"]).UserID,
                               FirstName = tbFirstName.Text,
                               LastName = tbLastName.Text,
                               Gender = ddlGender.SelectedValue,
                               Email =  tbEmail.Text,
                               City =  tbCity.Text,
                               State = tbState.Text,
                               Age = int.Parse(tbAge.Text),
                               Height = int.Parse(tbHeight.Text),
                               Weight = int.Parse(tbWeight.Text),
                               Wall = ((User)Session["userDetails"]).Wall
                           };

            if (pictureFile.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(pictureFile.FileName);
                if (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png")
                {
                    try
                    {
                        string imgPath = getUploadPath(user.UserID, fileExtension);
                        pictureFile.SaveAs(imgPath);
                       
                        resizeIcon(imgPath, user.UserID);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else if (deletePhoto.Checked)
            {
                deletePhotos(user);
            }

            if (UserDAO.UpdateUserProfile(user))
            {
                Session["userDetails"] = UserDAO.GetUserFromUserId(user.UserID);  
                Response.Redirect("~/Account/Profile.aspx");
            }
            else
            {
                Response.Redirect("~/Account/Profile.aspx");    
            }

        }

        /// <summary>
        /// Gets the absolute path to an user image directory on the server.
        /// </summary>
        /// <example>
        /// To get the path to the "icon" folder of user images.
        /// <code>
        /// string iconPath = userImageDirPath("icon");
        /// </code>
        /// Would return "~\Images\Account\UserProfile\icon\".
        /// </example>
        /// <param name="dirName">Name of the directory. Typically you can just use the name of an image size.</param>
        /// <returns></returns>
        private string imageDirPath(string dirName)
        {
            return String.Format("{0}\\Images\\Account\\UserProfile\\{1}\\", Server.MapPath("~"), dirName);
        }

        /// <summary>
        /// Like <see cref="imageDirPath"/> except will give the exact directory the users image is in.
        /// </summary>
        /// <param name="dirName">Name of the directory. Typically you can just use the name of an image size.</param>
        /// <param name="userID">The ID of the user.</param>
        /// <returns></returns>
        private string userImageDirPath(string dirName, int userID)
        {
            int userDir = (userID / 1000);
            return String.Format("{0}\\{1}\\", imageDirPath(dirName), userDir);
        }

        /// <summary>
        /// Gets the absolute path to the upload directory for user photos.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="fileExtension">The extension of the file to retrieve the path for.</param>
        private string getUploadPath(int userID, string fileExtension)
        {
            string imgName = userID + fileExtension;

            string path = imageDirPath("Original");
            string uploadDir = userImageDirPath("Original", userID);
            string uploadPath = uploadDir + imgName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            return uploadPath;
        }

        /// <summary>
        /// Gets the absolute path to a particular image of named size.
        /// 
        /// Current sizes:
        ///   - icon: a 150x150 icon for the user
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="sizeName">The name of the icon size.</param>
        /// <param name="fileExtension">The extension of the file to retrieve the path for.</param>
        /// <returns></returns>
        private string getSizedIconPath(int userID, string sizeName, string fileExtension)
        {
            string imgName = userID + fileExtension;

            string path = imageDirPath(sizeName);
            string uploadDir = userImageDirPath(sizeName, userID);
            string uploadPath = uploadDir + imgName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }


            return uploadPath;
        }

        /// <summary>
        /// Creates an image of size "icon".
        /// </summary>
        /// <param name="originalPath">The full path to the original image.</param>
        /// <param name="userID">The ID of the user to generate the icon for.</param>
        private void resizeIcon(string originalPath, int userID)
        {
            createSquarePhotoSize(originalPath, "icon", userID, 150);
        }


        /// <summary>
        /// Creates a square cut of the image at <paramref name="originalPath"/> and places it in
        /// /Images/Account/UserProfile/<paramref name="sizeName"/>/*subdirectory*/<paramref name="userID"/>.png
        /// </summary>
        /// <param name="originalPath">The path to the original image to resize.</param>
        /// <param name="sizeName">A name for the size, it will be put in that directory. Will be created if it does not exist.</param>
        /// <param name="userID">The ID of the user to generate the image for. The photo will be put in a subdirecory of sizename based on userID.</param>
        /// <param name="squareImgLen">The side length of the square.</param>
        private void createSquarePhotoSize(string originalPath, string sizeName, int userID, int squareImgLen)
        {
            string newFilePath = getSizedIconPath(userID, sizeName, ".png");

            Stream inFile = null;
            Stream outFile = null;

            try
            {
                inFile = new FileStream(originalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                outFile = new FileStream(newFilePath, FileMode.Create, FileAccess.Write, FileShare.Write);

                using (var img = System.Drawing.Image.FromStream(inFile))
                using (var icon = new System.Drawing.Bitmap(squareImgLen, squareImgLen))
                {
                    var smallerSide = Math.Min(img.Width, img.Height);
                    var centerX = (img.Width - smallerSide) / 2;
                    var centerY = (img.Height - smallerSide) / 2;

                    using (var iconGraphic = System.Drawing.Graphics.FromImage(icon))
                    {
                        iconGraphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        iconGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        iconGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        var cutRect = new System.Drawing.Rectangle(0, 0, squareImgLen, squareImgLen);
                        iconGraphic.DrawImage(img, cutRect, centerX, centerY, smallerSide, smallerSide, System.Drawing.GraphicsUnit.Pixel);
                        icon.Save(outFile, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (inFile != null)
                {
                    inFile.Close();
                }
                if (outFile != null)
                {
                    outFile.Close();
                }
            }
        }

        private void deletePhotos(User user)
        {
            string[] newPhotoSizes = {"icon"};

            foreach (string photoSize in newPhotoSizes)
            {
                string userPhoto = userImageDirPath(photoSize, user.UserID) + user.UserID + ".png";
                if (File.Exists(userPhoto))
                {
                    File.Delete(userPhoto);
                }
            }

            string[] files = Directory.GetFiles(userImageDirPath("Original", user.UserID), user.UserID + ".*");
            if (files.Length > 0)
            {
                File.Delete(files[0]);
            }
        }
    }
}