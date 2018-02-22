using System.Collections.Generic;

namespace ServiceRequest.DependencyInterfaces

{
    public interface IMapView
    {
        void LoadPin(object pin);
        void LoadPins<T>(List<T> pinList);
        bool ClearPin();
        void MoveToRegion(object position, object distance);
		void DisposeMap();
    }
}
