/*
    Interface that require all root states to hold variables/functions. Currently only deals with gravity
    
    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

public interface IRootState 
{
    void HandleGravity();
}
