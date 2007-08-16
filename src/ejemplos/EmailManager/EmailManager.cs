// MyClass.cs created with MonoDevelop
// User: nacho at 13:49 15/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 15/08/2007 at 13:49
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

public class EmailManager
{

	public void enviarEmail(String dest, String asunto, String cuerpo, String gm_username, String gm_password) {
			try {
				SmtpClient smtpCli = new SmtpClient("smtp.gmail.com",587);
				NetworkCredential authInfo = new NetworkCredential(gm_username, gm_password);
				smtpCli.UseDefaultCredentials = false;
				smtpCli.EnableSsl = true;
				smtpCli.Credentials = authInfo;
				smtpCli.Timeout = 15000;
				try {
	   				smtpCli.Send(gm_username+"@gmail.com",dest,asunto,cuerpo);
	 			} catch (Exception ex) {
					
				}
 			} catch (Exception ex) {
				
			}
	}
}