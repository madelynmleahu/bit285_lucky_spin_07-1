using System;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        //TODO: remove reference to the Singleton Repository
        //      and inject a reference (dbcRepo) to the LuckySpinContext 
        private LuckySpinContext dbcRepo; // DONE
        Random random = new Random();

        /***
         * Controller Constructor
         */
        public SpinnerController(LuckySpinContext dbcRepoo) //  DONE
        {
            dbcRepo = dbcRepoo; 
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

            //Create a new Player object //DONE
            Player player = new Player
            {
                FirstName = info.FirstName,
                Luck = info.Luck,
                Balance = info.StartingBalance
            };
            //TODO: Update persistent data using dbcRepo.Players.Add() and SaveChanges()

            dbcRepo.Players.Add(player); //DONE
            dbcRepo.SaveChanges(); //DONE

            //TODO: Pass the player Id to SpinIt
            //return RedirectToAction("SpinIt");  DELETE
            return RedirectToAction("SpinIt", new { id = player.Id }); //DONE
        }

        /***
         * Play through one Spin
         **/  
         [HttpGet]      
         public IActionResult SpinIt(long Id) //TODO: receive the player Id  (player.Id) ?? ***************** (int id) //DONE
        {
            //TODO: Use the dbcRepo.Player.Find() to get the player object
            Player player = dbcRepo.Players.Find(Id); //******************* //DONE ????????????????????? COME BACK HERE FOR CHECKING

            //TODO: Intialize the spinItVM with the player object from the database
            SpinItViewModel spinItVM = new SpinItViewModel() {
                FirstName = player.FirstName,  //DONE
                Luck = player.Luck,
                Balance = player.Balance
            };

            if (!spinItVM.ChargeSpin())
            {
                return RedirectToAction("LuckList");
            }

            if (spinItVM.Winner) { spinItVM.CollectWinnings(); }

            // TODO: Update the player Balance using the Player from the database
            //repository.CurrentPlayer.Balance = spinItVM.Balance;
            player.Balance = spinItVM.Balance; //CANNOT FIGURE OUT HOW TO DO THIS, WONT COMPILE

            //Store the Spin in the Repository
            Spin spin = new Spin()
            {
                IsWinning = spinItVM.Winner
            };
            //TODO: Update persistent data using dbcRepo.Spins.Add() and SaveChanges()
            //repository.AddSpin(spin); DELETE
            dbcRepo.Spins.Add(spin); //DONE
            dbcRepo.SaveChanges(); //DONE

            return View("SpinIt", spinItVM);
        }

        /***
         * ListSpins Action
         **/
         [HttpGet]
         public IActionResult LuckList()
        {
            //TODO: Pass the View the Spins collection from the dbcRepo
            //return View(repository.PlayerSpins); DELETE
            return View(dbcRepo.Spins); //DONE
        }

    }
}

