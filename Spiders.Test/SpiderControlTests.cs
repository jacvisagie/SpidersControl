using Microsoft.AspNetCore.Mvc.RazorPages;
using Spiders.Command.Pages;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spiders.Test
{
    public class SpiderControlTests
    {
        [Fact]
        public async Task Test_Post_Input_Correct_Pass()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = "4 10 Left",
                    Input3 = "FLFLFRFFLF"
                }
            };

            var OutputStr1 = "5 7 Right";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);
        }

        [Fact]
        public async Task Test_Post_Input_InCorrect_Input_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = "3 10 Left",
                    Input3 = "FLFLFRFFLF"
                }
            };

            var OutputStr1 = "5 7 Right";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.NotEqual(OutputStr1, controller.Result);
        }

        [Fact]
        public async Task Test_Post_Input_InCorrect_Orrientation_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = "4 10 Lllleft",
                    Input3 = "FLFLFRFFLF"
                }
            };

            var OutputStr1 = "Spider nav failed - Spider journey input data incorrect or went off grid.";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);
        }

        [Fact]
        public async Task Test_Post_Input_StartPosision_Offgrid_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = "8 10 Left",
                    Input3 = "FLFLFRFFLF"
                }
            };

            var OutputStr1 = "Spider nav failed - Spider journey input data incorrect or went off grid.";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);

        }

        [Fact]
        public async Task Test_Post_Input_EndPosision_Offgrid_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = "4 10 Left",
                    Input3 = "FLFLFRFFLFFFFFFFFFFFF"
                }
            };

            var OutputStr1 = "Spider nav failed - Spider journey input data incorrect or went off grid.";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);
        }

        [Fact]
        public async Task Test_Post_InputStr1_Null_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = null,
                    Input2 = "4 10 Left",
                    Input3 = "FLFLFRFFLF"
                }
            };

            var OutputStr1 = "Spider nav failed - Null input";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);

        }

        [Fact]
        public async Task Test_Post_InputStr2_Null_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = null,
                    Input3 = "FLFLFRFFLF"
                }
            };

            var OutputStr1 = "Spider nav failed - Null input";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);

        }

        [Fact]
        public async Task Test_Post_InputStr3_Null_Fail()
        {
            //Arrange
            var controller = new IndexModel()
            {
                Input = new InputStringsViewModel
                {
                    Input1 = "7 15",
                    Input2 = "4 10 Left",
                    Input3 = null
                }
            };

            var OutputStr1 = "Spider nav failed - Null input";

            //Act
            _ = await controller.OnPostAsync("Submit");

            //Assert
            Assert.Equal(OutputStr1, controller.Result);

        }
    }
}
