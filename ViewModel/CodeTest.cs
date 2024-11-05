using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace MinimarketApp.ViewModel
{
    public class CodeTest
    {
        public class Caleg
        {
            public string Nama { get; set; }
            public string Partai { get; set; }
            public int? NoUrut { get; set; }

            public Caleg(string nama, string partai)
            {
                Nama = nama;
                Partai = partai;
            }

        }

        private List<Caleg> _calegs;

        public List<Caleg> Calegs { get => _calegs; set => _calegs = value; }

        public void RandomizeNoUrut()
        {
            for (int i = 0; i < Calegs.Count; i++)
            {
                Random random = new Random();
                int NoUrut = random.Next(1, Calegs.Count);
                if (IsNoUrutAvailable(NoUrut))
                {
                    Calegs[i].NoUrut = NoUrut;
                }
                else
                {
                    i--;
                }
            }
        }

        public void AddCaleg(string nama, string partai)
        {
            Calegs.Add(new Caleg(nama, partai));
        }

        public bool IsNoUrutAvailable(int noUrut)
        {
            for (int i = 0; i < Calegs.Count; i++)
            {
                if (Calegs[i].NoUrut == noUrut)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
