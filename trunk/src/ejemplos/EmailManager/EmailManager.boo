// EmailManager.boo
// User: nacho at 13:49 15/08/2007
//
//
// project created on 15/08/2007 at 13:49
namespace Prueba
import System
import System.IO
import System.Net
import System.Net.Mail


class EmailManager():
	def enviarGEmail(dest as String, asunto as String, cuerpo as String, gm_username as String, gm_password as String):
		try:
			smtpCli = SmtpClient("smtp.gmail.com",587)
			authInfo = NetworkCredential(gm_username, gm_password)
			smtpCli.UseDefaultCredentials = false
			smtpCli.EnableSsl = true
			smtpCli.Credentials = authInfo
			smtpCli.Timeout = 15000
			try:
   				smtpCli.Send(gm_username+"@gmail.com",dest,asunto,cuerpo)
 			except ex:
				pass
		except ex:
			pass		
