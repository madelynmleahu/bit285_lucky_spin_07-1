using System;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;
//using Microsoft.EntityFrameworkCore; ???

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        //TODO: remove reference to the Singleton Repository
        //      and inject a reference (dbcRepo) to the LuckySpinContext 
        private Repository repository; //************ DELETE ?
        private LuckySpinContext dbcRepo; //***********
        Random random = new Random();

        /***
         * Controller Constructor
         */
        public SpinnerController(Repository r) // LuckSpinContext dbcRepo2 ********
        {
            repository = r; // ************
            //dbcRepo = dbcRepo2 
        }

        /***
         * Entry Page Action
         **/

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel info)
        {
            if (!ModelState.IsValid) { return View(); }

            //Create a new Player object
            Player player = new Player
            {
                FirstName = info.FirstName,
                Luck = info.Luck,
                Balance = info.StartingBalance
            };
            //TODO: Update persistent data using dbcRepo.Players.Add() and SaveChanges()
            repository.CurrentPlayer = player;
            //dbcRepo.Player.Add(player); *********
            //dbcRepo.SaveChanges(); ********

            //TODO: Pass the player Id to SpinIt
            return RedirectToAction("SpinIt"); //return RedirectToAction("SpinIt", new { id = player.Id } ); ********
        }

        /***
         * Play through one Spin
         **/  
         [HttpGet]      
         public IActionResult SpinIt() //TODO: receive the player Id  (player.Id) ?? ***************** (int id)
        {
            //TODO: Use the dbcRepo.Player.Find() to get the player object
            //dbcRepo.Player.Find(([id]) *******************

            //TODO: Intialize the spinItVM with the player object from the database
            SpinItViewModel spinItVM = new SpinItViewModel() {
                FirstName = repository.CurrentPlayer.FirstName,  //Player = dbcRepo.Player ?????? ***********
                Luck = repository.CurrentPlayer.Luck,
                Balance = repository.CurrentPlayer.Balance
            };


            if (!spinItVM.ChargeSpin())
            {
                return RedirectToAction("LuckList");
            }


            if (spinItVM.Winner) { spinItVM.CollectWinnings(); }


            // TODO: Update the player Balance using the Player from the database
            repository.CurrentPlayer.Balance = spinItVM.Balance;

            //Store the Spin in the Repository
            Spin spin = new Spin()
            {
                IsWinning = spinItVM.Winner
            };
            //TODO: Update persistent data using dbcRepo.Spins.Add() and SaveChanges()
            repository.AddSpin(spin);
            //dbcRepo.Spins.Add(spin); *********
            //dbcRepo.SaveChanges(); **********

            return View("SpinIt", spinItVM);
        }

        /***
         * ListSpins Action
         **/
         [HttpGet]
         public IActionResult LuckList()
        {
            //TODO: Pass the View the Spins collection from the dbcRepo
            return View(repository.PlayerSpins); //dbcRepo.Spins **********
        }

    }
}

