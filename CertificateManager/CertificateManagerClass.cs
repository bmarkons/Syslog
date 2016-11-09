using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CertificateManager
{
	public class CertificateManagerClass
	{
		public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
		{
			X509Store store = new X509Store(storeName, storeLocation);
			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);// daj mi sve validne sertifikate



			if (collection.Count != 0)
			{
				return collection[0];
			}
			else
			{
				return null;
			}
		}

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
