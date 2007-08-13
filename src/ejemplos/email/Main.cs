/*
 * Created by SharpDevelop.
 * User: usuario
 * Date: 12/03/2007
 * Time: 12:35 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NotasDai
{
	class MainClass
	{
		private string emailPara;


		public static void Main(string[] args)
		{
			string sURLSaldo = "http://www.frsf.utn.edu.ar/matero/visitante/index.php?id_catedra=69&ver=8";
			//string sURLLogin = "http://www.servicios.cti.com.ar/webcustomer3/servlet/Controller?EVENT=LOGINMAS&loginNumber="+this.loginNumber+"&password="+this.loginPass;
			//string sURLLogout = "http://www.servicios.cti.com.ar/webcustomer3/servlet/Controller?EVENT=WELCOMEMAS";
			
			CookieContainer cookies;
			string pagina;
			string urlNota;
			string nota;
			

			cookies = getCookies(sURLSaldo);
			pagina = getPagina(sURLSaldo,cookies);
			urlNota = getUrlNota(pagina);

			if (!urlNota.Equals("false")) {
				pagina = getPagina("http://www.frsf.utn.edu.ar/matero/visitante/"+urlNota,cookies);
				nota = getNota(pagina);
				Console.WriteLine(nota);
				int notaint = int.Parse(nota);
				if (notaint >= 4) {
					EnviarEmail(nota,"otroemail@gmail.com");
				}
				EnviarEmail(nota,"miemail@midominio.com");
		
			}

		}
		public static CookieContainer getCookies(string url) {
			// Login
			HttpWebRequest login;
			login = (HttpWebRequest) HttpWebRequest.Create(url);
			login.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705)";
			login.KeepAlive = true;	
			login.CookieContainer = new CookieContainer();
			login.GetResponse().Close();
			CookieContainer cookies = login.CookieContainer;
			return cookies;
		}
		public static string getPagina(string url, CookieContainer cookies) {
			// Pagina de Saldo
			Console.WriteLine("Ingresando en..."+url);
			HttpWebRequest saldo = (HttpWebRequest) HttpWebRequest.Create(url);			
			saldo.KeepAlive = true;	
			saldo.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705)";
			saldo.CookieContainer = cookies;
            StreamReader saldoReader = new StreamReader(saldo.GetResponse().GetResponseStream());
            
            string sLine = "";
            string ret = "";
			while (sLine!=null)	{
            	sLine = saldoReader.ReadLine();
				ret += sLine;
			}
            saldo.GetResponse().Close();
            return ret;
		}
		public static string getUrlNota(string contPagina) {
			Console.WriteLine("Buscando la URL de las notas del examen del 27 de julio...");
			Match m = Regex.Match(contPagina,@"27/07/07\s*</strong>\s*</div>\s*</td>\s*<td width=""18%"">\s*<div align=""center"">\s*<a href='(?<saldito>[^']+)'>Notas");
			if (m.Success) {
				Console.WriteLine("Encontrado. En: "+m.Result("${saldito}"));
				return m.Result("${saldito}");
			} else {
					return "false";
			}
				
		}
		public static string getNota(string contPagina) {
			Match m = Regex.Match(contPagina,@"15093</td>\s*<td width=""16%"">\s*<div align=""center"">(?<nota>[^<]+)</div>");
			if (m.Success)
				return m.Result("${nota}"); 
			else 
				return "false";
		}
		
		public static void EnviarEmail(string nota,string dest) {
			try {
				//Mailer.Send("Hola","Tu saldo es de: " + saldo,this.emailPara);
				SmtpClient smtpCli = new SmtpClient("smtp.gmail.com",587);
				NetworkCredential authInfo = new NetworkCredential("gmail_login_user", "passwordXXXXX");
				smtpCli.UseDefaultCredentials = false;
				smtpCli.EnableSsl = true;
				smtpCli.Credentials = authInfo;
				smtpCli.Timeout = 15000;
				try {
	   				smtpCli.Send("elcelularloco@gmail.com",dest,"estan las notas de SO","sacaste: "+nota+"!!!\nFelicitaciones!(soy un programa escrito por Nacho)");
	 			} catch (Exception ex) {
					
				}
 			} catch (Exception ex) {
				
			}
		}
	}
}
