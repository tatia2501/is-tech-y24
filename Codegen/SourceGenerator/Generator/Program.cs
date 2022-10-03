using MyEntities;
using MyClient;
namespace Generator;

public static class Program
{
    static void Main(string[] args)
    {
        var pet = new Pet();
        var client = new Client();
        //Console.WriteLine(Client.getPet(2));
    }
}
/*
        var owner1 = new Owner
        {
            id = 3,
            name = "Owner3",
            date = "23.09.1999"
        };
        //Client.postOwner(owner1);
        
        var pet = new Pet
        {
            animal = "dachshund",
            date = "16.07.2009",
            id = 3,
            name = "Pet3",
            owner = owner1
        };
        //Client.postPet(pet);
*/