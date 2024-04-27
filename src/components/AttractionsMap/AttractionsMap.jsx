import * as React from 'react'
import { MapContainer, TileLayer, useMap, Popup, Marker } from 'react-leaflet'
import Pin from '../Pin/Pin'

function AttractionsMap({attractions}) {
    
    let position = [0, 0] 

    if(attractions.length != 0)  position = [attractions[0].longitude, attractions[0].latitude]

    return <MapContainer center={position} zoom={13} scrollWheelZoom={false} style={{height: "100%", width: "100%", borderRadius: 50}}>
    <TileLayer
      attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    />
    {attractions.map(attraction => <Pin key={attraction.id} attraction={attraction}/>)}
  </MapContainer>
}

export default AttractionsMap;