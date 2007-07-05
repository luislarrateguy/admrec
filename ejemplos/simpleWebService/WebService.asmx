<%@ WebService Language="C#" class="SecurityWebService" %>

using System;
using System.Web.Services;

public struct SecurityInfo
{
		public string Code;
		public string CompanyName;
		public double Price;
}

public class SecurityWebService : WebService
{
	private SecurityInfo Security;
	
	public SecurityWebService()
	{
		Security.Code = "";
		Security.CompanyName = "";
		Security.Price = 0;
	}

	private void AssignValues(string Code)
	{
		// This is where you use your business components. 
		// Method calls on Business components are used to populate the data.
		// For demonstration purposes, I will add a string to the Code and
		//		use a random number generator to create the price feed.
					
		Security.Code = Code;
		Security.CompanyName = Code + " Pty Ltd";
		Random RandomNumber = new System.Random();
		Security.Price = double.Parse(new System.Random(RandomNumber.Next(1,10)).NextDouble().ToString("##.##"));
	}


    [WebMethod(Description="This method call will get the company name and the price for a given security code.",EnableSession=false)]
	public SecurityInfo GetSecurityInfo(string Code)
	{
		AssignValues(Code);
		SecurityInfo SecurityDetails = new SecurityInfo();
		SecurityDetails.Code = Security.Code;
		SecurityDetails.CompanyName = Security.CompanyName;
		SecurityDetails.Price = Security.Price;
		return 	SecurityDetails;
	}

}

