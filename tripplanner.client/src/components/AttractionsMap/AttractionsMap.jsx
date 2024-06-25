import * as React from 'react'
import { MapContainer, TileLayer, useMap, Popup, Marker } from 'react-leaflet'
import Pin from '../Pin/Pin'

function AttractionsMap({attractions}) {
    
    if(attractions == null) attractions = []

    let position = [0, 0] 
    let zoom = 100

    if(attractions.length != 0)  {
       let minLong = 1000;
       let maxLong = -1000;
       let minLat = 1000;
       let maxLat = -1000;

       for(var att of attractions){
        if(att.longitude < minLong) minLong = att.longitude;
        if(att.longitude > maxLong) maxLong = att.longitude;
        if(att.latitude < minLat) minLat = att.latitude;
        if(att.latitude > maxLat) maxLat = att.latitude;
       }
     
       let latLen = Math.abs(maxLat-minLat)
       let longLen = Math.abs(maxLong-minLong)
       position = [(maxLat+minLat)/2, (maxLong+minLong)/2]

       let leng = Math.max(latLen, longLen)
       if(leng > 20){
        zoom = leng * 0.05;
       } else if (leng > 5){
        zoom = leng * 0.4;
       } else if (leng > 2){
        zoom = leng * 3;
       } else {
        zoom = leng * 5;
       }
       
    }

    return <MapContainer center={position} zoom={zoom} scrollWheelZoom={false} style={{height: "100%", width: "100%", borderRadius: 50}}>
    <TileLayer
      attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    />
    {attractions.map(attraction => <Pin key={attraction.id} attraction={attraction}/>)}
  </MapContainer>
}

export default AttractionsMap;