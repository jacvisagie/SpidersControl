using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spiders.Command.Pages
{
    public class IndexModel : PageModel
    {
        private string[] headNme = { "Left", "Up", "Right", "Down" };
        private int[] headVal = { 0, 1, 2, 3 };
        private int orrentMem = -1;
        private int xMaxMem = -1;
        private int yMaxMem = -1;
        private int xStartMem = -1;
        private int yStartMem = -1;

        [BindProperty]
        public InputStringsViewModel Input { get; set; }

        public string Result { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        private bool GetPosition(string str1, string str2, string str3)
        {
            var success = true;

            if ((str1 != null) && (str2 != null) && (str3 != null))
            {
                try
                {
                    //setting grid limits
                    success = Setwall(str1);

                    //if setwall fail - set response
                    if (!success)
                        Result = "Wall size input data incorrect.";

                    //if successfull so far - set start position for spider
                    if (success)
                        success = SetSpiderStart(str2);

                    //if SetSpiderStart fail - set response
                    if (!success)
                        Result = "Spider start position input data incorrect or not on grid.";

                    if (success)
                        success = DoJourney(str3);

                    //if DoJourney fail - set response
                    if (!success)
                        Result = "Spider journey input data incorrect or went off grid.";

                    //get final position
                    if (success)
                        success = FindFinalPosition();
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            else
            {
                success = false;
                Result = "Null input";
            }

            if (!success)
                Result = $"Spider nav failed - {Result}";

            return success;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var success = GetPosition(Input.Input1, Input.Input2, Input.Input3);

            return Page();
        }

        private bool FindFinalPosition()
        {
            try
            {
                //string up final position
                Result = $"{xStartMem} {yStartMem} {headNme[orrentMem]}";
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool SetSpiderStart(string str2)
        {
            var rtrn = true;

            //splitting string to array and then parsing values is ints
            var arr = str2.Split(' ');

            rtrn = Int32.TryParse(arr[0], out xStartMem);

            //if x parse successful, check on grid
            if (rtrn)
            {
                rtrn = xStartMem <= xMaxMem ? true : false;
            }

            //if x on grid, try y
            if (rtrn)
            {
                rtrn = Int32.TryParse(arr[1], out yStartMem);

                //if y parse successful, check on grid
                if (rtrn)
                {
                    rtrn = yStartMem <= yMaxMem ? true : false;
                }

                //if y on grid
                if (rtrn)
                {
                    //find orientation in orientation names array
                    var pos = Array.IndexOf(headNme, arr[2]);

                    //if exists, save index value
                    if (pos > -1)
                    {
                        orrentMem = headVal[pos];
                    }
                    else
                    {
                        rtrn = false;
                    }
                }

            }

            return rtrn;
        }

        private bool Setwall(string str1)
        {
            var rtrn = true;

            //finding space in string
            var s1 = str1.IndexOf(" ");

            //if exists
            if (s1 != -1)
            {
                //pars x val as int, set as outer edge of wall
                rtrn = Int32.TryParse(str1.Substring(0, s1), out xMaxMem);

                //if success, parse y, set as top of wall
                if (rtrn)
                    rtrn = Int32.TryParse(str1.Substring(s1++, str1.Length - 1), out yMaxMem);
            }

            return rtrn;
        }

        private bool DoJourney(string str3)
        {
            var rslt = true;

            var arr = str3.ToCharArray();

            for (int i = 0; i < arr.Length; i++)
            {
                if (rslt)
                {
                    //if heading remains the same, move 1 grid pos in current direction
                    if (arr[i] == 'F')
                    {
                        //increase/decrease x or y positions based on direction of travel
                        switch (orrentMem)
                        {
                            case 0:

                                //decrease x
                                xStartMem--;

                                break;

                            case 1:

                                //increase y
                                yStartMem++;

                                break;

                            case 2:

                                //increase x
                                xStartMem++;

                                break;

                            case 3:

                                //decrease y
                                yStartMem--;

                                break;

                            default:
                                break;
                        }

                        //check if forward movement has forced spider of the grid
                        if (rslt)
                        {
                            rslt = xStartMem <= xMaxMem ? true : false;
                        }

                        if (rslt)
                        {
                            rslt = yStartMem <= yMaxMem ? true : false;
                        }

                    }

                    //using simple int array (headVal) as reference to direction of travel
                    //0 - left
                    //1 - up
                    //2 - right
                    //3 - down

                    //rotating through headval in order to select and record the next direction after a 90 degree turn

                    //if turning left
                    if (arr[i] == 'L')
                    {
                        //decrease orrentMem
                        orrentMem--;

                        //if orrentMem bottomed out - go to top
                        if (orrentMem == -1)
                        {
                            orrentMem = 3;
                        }
                    }

                    //if turning right
                    if (arr[i] == 'R')
                    {
                        //increment orrentMem
                        orrentMem++;

                        //if index topped out - go to bottom
                        if (orrentMem == 4)
                        {
                            orrentMem = 0;
                        }
                    }
                }
            }

            return rslt;
        }
    }

    public class InputStringsViewModel
    {
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }

    }
}
