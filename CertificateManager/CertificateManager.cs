using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CertificateManager
{
	public class CertificateManager
	{
		/// <summary>
		/// Get certificate from file from SolutionPath\\CertificateManager\\Certificates\\ 
		/// </summary>
		/// <param name="certName">Name of file(.pfx)</param>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static X509Certificate2 GetCertificateFromFile(string certName, string pass)
		{
			string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            string certPath = solutionPath + "\\CertificateManager\\Certificates\\" + certName;

			X509Certificate2Collection collection = new X509Certificate2Collection();
			collection.Import(certPath, pass, X509KeyStorageFlags.PersistKeySet);

			if (collection.Count != 0)
			{
				return collection[0];
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// Get certificate from file from SolutionPath\\CertificateManager\\Certificates\\ 
		/// </summary>
		/// <param name="certName">Name of file(.cer)</param>
		/// <returns></returns>
		public static X509Certificate2 GetCertificateFromFile(string certName)
		{

			string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            string certPath = solutionPath + "\\CertificateManager\\Certificates\\" + certName;

			X509Certificate2Collection collection = new X509Certificate2Collection();
			collection.Import(certPath);

			if (collection.Count != 0)
			{
				return collection[0];
			}
			else
			{
				return null;
			}
		}
	}
}
