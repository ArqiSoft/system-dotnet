using System;
using System.Web.Profile;

namespace Sds.XmlProfile
{
    public static class ProfileExtensions
	{
		public static string FirstName(this ProfileBase profile)
		{
			try
			{
				return profile.GetPropertyValue("FirstName").ToString();
			}
			catch(Exception)
			{
				return string.Empty;
			}
		}

		public static string LastName(this ProfileBase profile)
		{
			try
			{
				return profile.GetPropertyValue("LastName").ToString();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		public static string DisplayName(this ProfileBase profile)
		{
			try
			{
				return profile.GetPropertyValue("DisplayName").ToString();
			}
			catch (Exception)
			{
				return profile.FirstName() + " " + profile.LastName();
			}
		}

		public static string Email(this ProfileBase profile)
		{
			try
			{
				return profile.GetPropertyValue("Email").ToString();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}
}
