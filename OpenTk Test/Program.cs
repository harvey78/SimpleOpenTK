using OpenTk_Test;


// This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.


using Game game = new Game("Simple OpenTK", 1920, 1080);
{
    //Run takes a double, which is how many frames per second it should strive to reach.
    //You can leave that out and it'll just update as fast as the hardware will allow it.
    game.Run();
}


