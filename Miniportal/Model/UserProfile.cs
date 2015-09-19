using System;

namespace Es.Udc.DotNet.MiniPortal.Model
{
    public partial class UserProfile 
    {
     
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {

            UserProfile target = (UserProfile)obj;

            return (this.usrId == target.usrId)
                   && (this.loginName == target.loginName)
                   && (this.enPassword == target.enPassword)
                   && (this.firstName == target.firstName)
                   && (this.lastName == target.lastName)
                   && (this.email == target.email);
        }

        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, it is based on a field that does not change.
        public override int GetHashCode()
        {
            return this.usrId.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strUserProfile;

            strUserProfile =
                "usrId = " + this.usrId + " | " +
                "loginName = " + this.loginName + " | " +
                "firstName = " + this.firstName + " | " +
                "lastName = " + this.lastName + " | " +
                "email = " + this.email;

            return strUserProfile;
        }
    }
}
