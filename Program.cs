using Module3.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module3
{
    class Program
    {
        private static List<Auteur> ListeAuteurs = new List<Auteur>();
        private static List<Livre> ListeLivres = new List<Livre>();

        private static void InitialiserDatas()
        {
            ListeAuteurs.Add(new Auteur("GROUSSARD", "Thierry"));
            ListeAuteurs.Add(new Auteur("GABILLAUD", "Jérôme"));
            ListeAuteurs.Add(new Auteur("HUGON", "Jérôme"));
            ListeAuteurs.Add(new Auteur("ALESSANDRI", "Olivier"));
            ListeAuteurs.Add(new Auteur("de QUAJOUX", "Benoit"));
            ListeLivres.Add(new Livre(1, "C# 4", "Les fondamentaux du langage", ListeAuteurs.ElementAt(0), 533));
            ListeLivres.Add(new Livre(2, "VB.NET", "Les fondamentaux du langage", ListeAuteurs.ElementAt(0), 539));
            ListeLivres.Add(new Livre(3, "SQL Server 2008", "SQL, Transact SQL", ListeAuteurs.ElementAt(1), 311));
            ListeLivres.Add(new Livre(4, "ASP.NET 4.0 et C#", "Sous visual studio 2010", ListeAuteurs.ElementAt(3), 544));
            ListeLivres.Add(new Livre(5, "C# 4", "Développez des applications windows avec visual studio 2010", ListeAuteurs.ElementAt(2), 452));
            ListeLivres.Add(new Livre(6, "Java 7", "les fondamentaux du langage", ListeAuteurs.ElementAt(0), 416));
            ListeLivres.Add(new Livre(7, "SQL et Algèbre relationnelle", "Notions de base", ListeAuteurs.ElementAt(1), 216));
            ListeAuteurs.ElementAt(0).addFacture(new Facture(3500, ListeAuteurs.ElementAt(0)));
            ListeAuteurs.ElementAt(0).addFacture(new Facture(3200, ListeAuteurs.ElementAt(0)));
            ListeAuteurs.ElementAt(1).addFacture(new Facture(4000, ListeAuteurs.ElementAt(1)));
            ListeAuteurs.ElementAt(2).addFacture(new Facture(4200, ListeAuteurs.ElementAt(2)));
            ListeAuteurs.ElementAt(3).addFacture(new Facture(3700, ListeAuteurs.ElementAt(3)));
        }

        private static void displayAuthor(Auteur author, string prefix = "")
        {
            Console.WriteLine(prefix + "  " + author.Prenom + " " + author.Nom);
        }

        private static void displayAuthorDict<T>(Dictionary<Auteur, T> dict, string prefix = "")
        {
            foreach (KeyValuePair<Auteur, T> item in dict)
            {
                Auteur author = item.Key;
                Console.WriteLine(prefix + "  " + author.Prenom + " " + author.Nom + ": " + item.Value);
            }
        }

        private static void displayIEnumerable(IEnumerable<string> list, string prefix = "")
        {
            foreach (string item in list)
            {
                Console.WriteLine(prefix + "  " + item);
            }
        }


        static void Main(string[] args)
        {
            InitialiserDatas();

            IEnumerable<IGrouping<Auteur, Livre>> booksByAuthor = ListeLivres.GroupBy(l => l.Auteur);

            #region Liste des prénoms des auteurs dont le nom commence par 'G'
            Console.WriteLine("Liste des prénoms des auteurs dont le nom commence par 'G':");
            IEnumerable<string> gLastNameAuthorsFirstName = ListeAuteurs.Where(a => a.Nom.StartsWith("G")).Select(a => a.Prenom);
            displayIEnumerable(gLastNameAuthorsFirstName);
            Console.WriteLine();
            #endregion

            #region Auteur ayant écrit le plus de livres
            Console.WriteLine("Auteur ayant écrit le plus de livres:");
            Auteur mostWrittenBooksAuthor = ListeLivres.GroupBy(l => l.Auteur).OrderByDescending(a => a.Count()).FirstOrDefault().Key;
            displayAuthor(mostWrittenBooksAuthor);
            Console.WriteLine();
            #endregion

            #region Nombre moyen de pages par livre par auteur
            Console.WriteLine("Nombre moyen de pages par livre par auteur:");
            Dictionary<Auteur, double> booksPageCountAvgByAuthor = new Dictionary<Auteur, double>();
            foreach (IGrouping<Auteur, Livre> booksForAuthor in booksByAuthor)
            {
                booksPageCountAvgByAuthor.Add(booksForAuthor.Key, booksForAuthor.Average(l => l.NbPages));
            }
            displayAuthorDict<double>(booksPageCountAvgByAuthor);
            Console.WriteLine();
            #endregion

            #region Titre du livre avec le plus de pages
            Console.WriteLine("Titre du livre avec le plus de pages:");
            string thickestBookTitle = ListeLivres.OrderByDescending(l => l.NbPages).FirstOrDefault().Titre;
            Console.WriteLine("  " + thickestBookTitle);
            Console.WriteLine();
            #endregion

            #region Gain moyen des auteurs
            Console.WriteLine("Gain moyen des auteurs:");
            Decimal allAuthorsGainAvg = ListeAuteurs.Average(a => a.Factures.Sum(f => f.Montant));
            Console.WriteLine("  Tous les auteurs:" + allAuthorsGainAvg);
            Dictionary<Auteur, decimal> avgGainByAuthor = new Dictionary<Auteur, decimal>();
            foreach (Auteur author in ListeAuteurs)
            {
                avgGainByAuthor.Add(author, author.Factures.Sum(f => f.Montant));
            }
            displayAuthorDict<decimal>(avgGainByAuthor, "  ");
            Console.WriteLine();
            #endregion

            #region Auteurs et liste de leurs livres
            Console.WriteLine("Auteurs et liste de leurs livres:");
            Dictionary<Auteur, string> booksStringByAuthor = new Dictionary<Auteur, string>();
            foreach (IGrouping<Auteur, Livre> booksForAuthor in booksByAuthor)
            {
                Auteur author = booksForAuthor.Key;
                string booksListString = string.Join(", ", booksForAuthor.Select(l => l.Titre).ToArray());
                booksStringByAuthor.Add(author, booksListString);
            }
            displayAuthorDict<string>(booksStringByAuthor);
            Console.WriteLine();
            #endregion

            #region Titres de tous les livres triés par ordre alphabétique
            Console.WriteLine("Titres de tous les livres triés par ordre alphabétique:");
            IEnumerable<string> sortedBooksTitles = ListeLivres.OrderByDescending(l => l.Titre).Select(l => l.Titre).Distinct();
            displayIEnumerable(sortedBooksTitles);
            #endregion

            #region Liste des livres dont le nombre de pages est supérieur à la moyenne
            Console.WriteLine("Liste des livres dont le nombre de pages est supérieur à la moyenne:");
            double pagesAvg = ListeLivres.Average(l => l.NbPages);
            IEnumerable<Livre> booksBiggerThanAvg = ListeLivres.Where(l => l.NbPages > pagesAvg);
            displayIEnumerable(booksBiggerThanAvg.Select(l => l.Titre));
            Console.WriteLine();
            #endregion

            #region Auteur ayant écrit le moins de livres
            Console.WriteLine("Auteur ayant écrit le moins de livres:");
            Auteur lestWrittenBooksAuthor = ListeAuteurs.OrderBy(a => ListeLivres.Count(l => l.Auteur == a)).FirstOrDefault();
            displayAuthor(lestWrittenBooksAuthor);
            #endregion

            Console.ReadKey();
        }
    }
}
