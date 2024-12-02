
using Photon.Realtime;

public interface IOnlineManager
{
        public ClientState OnlineState { get; }
        public bool IsRoomOwner { get; }
        
        public void Login();
        void Logout();
}