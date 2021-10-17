using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_genetique
{
    public class Person
    {
        public int daysSurvived;
        public string choices;
        public Person()
        {
            daysSurvived = 0;
            choices = "";
        }
    }
    class Program
    {
        static Random aleatoire = new Random();
        static string getChromosome(int size)
        {
            string chromosome = "";
            for (int i = 0; i < size; i++)
            {
                chromosome += aleatoire.Next(2);
            }
            return chromosome;
        }

        static void Main(string[] args)
        {
            int timeReproduceBoar=4;
            int daysBeforeDie = 5;
            int nbrIndividus = 100;
            int nbrGenerations = 10;
            //choice for the individues : kill a boar (1) or wait next day (0)
            Person[] pop = new Person[nbrIndividus];
            int daysToSurvive = 500;

            //definition
            for (int i = 0; i < nbrIndividus; i++)
            {
                pop[i] = new Person();
                pop[i].choices = getChromosome(daysToSurvive);
            }

            for (int g = 0; g < nbrGenerations; g++)
            {
                Console.WriteLine("____________ Generation " + g + " ____________");
                
                //evaluation (fitness = nombre de jours survécus)
                for (int i = 0; i < nbrIndividus; i++)
                {
                    //Console.WriteLine(pop[i].choices); //show all the choices
                    int daysWithoutEat = 0;
                    int meadow = 4; //start with 4 boars
                    pop[i].daysSurvived = 0;
                    foreach (var choice in pop[i].choices)
                    {
                        if ((pop[i].daysSurvived + 1) % timeReproduceBoar == 0)
                        {
                            meadow += (int)Math.Floor((double)meadow / 2);
                            //Console.WriteLine("The day "+ pop[i].daysSurvived + " had "+meadow+" boars.");
                        }
                        if (choice.Equals('0') || meadow == 0)
                        {
                            daysWithoutEat++;
                            if (daysWithoutEat == daysBeforeDie) break;
                        }
                        else
                        {
                            daysWithoutEat = 0;
                            meadow--;
                        }
                        pop[i].daysSurvived++;
                    }
                    Console.WriteLine("Number " + i + " survive " + pop[i].daysSurvived + " days with " + meadow + " boars.");
                }

                //séléction
                List<Person> newGeneration = new List<Person>();

                Array.Sort(pop, delegate (Person user1, Person user2)
                { //sort of the population, Elitism Selection
                    return user2.daysSurvived.CompareTo(user1.daysSurvived);
                });
                Console.WriteLine("Best : " + pop[0].daysSurvived);

                //we keep the best 15
                for (int i = 0; i < 15; i++)
                {
                    newGeneration.Add(pop[i]);
                }


                //croisement
                Person[] parents = (Person[])pop.Clone();
                while (newGeneration.Count < nbrIndividus)
                {
                    int individu1 = aleatoire.Next(pop.Length);
                    int individu2 = aleatoire.Next(pop.Length);
                    Person newPerson1 = new Person();
                    Person newPerson2 = new Person();
                    int ptSwitch = aleatoire.Next(daysToSurvive);
                    for (int j = 0; j < ptSwitch; j++)
                    {
                        newPerson1.choices += pop[individu1].choices[j];
                        newPerson2.choices += pop[individu2].choices[j];
                    }
                    for (int j = ptSwitch; j < daysToSurvive; j++)
                    {
                        newPerson2.choices += pop[individu1].choices[j];
                        newPerson1.choices += pop[individu2].choices[j];
                    }
                    newGeneration.Add(newPerson1);
                    if (newGeneration.Count != nbrIndividus) newGeneration.Add(newPerson2);
                }

                //mutation
                int probability = 100;// 1/100 chance
                int pos = 0;
                foreach (var chromosome in newGeneration)
                {
                    if (aleatoire.Next(probability + 1) == pos) //you get the mutation !!
                    {
                        Console.WriteLine("mutation !!");
                        int pos1 = aleatoire.Next(chromosome.choices.Length); //swap between two random position
                        int pos2 = aleatoire.Next(chromosome.choices.Length);
                        char temp = chromosome.choices[pos1];
                        StringBuilder sb = new StringBuilder(chromosome.choices);
                        sb[pos1] = chromosome.choices[pos2];
                        sb[pos2] = temp;
                        chromosome.choices = sb.ToString();
                    }
                    pop[pos] = chromosome;
                    pos++;
                }

                
            }

                Console.ReadKey();
        }
        

    }
}
