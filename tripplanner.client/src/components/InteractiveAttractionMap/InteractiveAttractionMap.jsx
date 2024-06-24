import * as React from 'react'
import { MapContainer, TileLayer } from 'react-leaflet'
import DraggableMarker from '../DraggableMarker/DraggableMarker'

function InteractiveAttractionMap({attraction, onChangePosition}) {
    
    let position = [attraction.latitude, attraction.longitude] 

    return <MapContainer center={position} zoom={13} scrollWheelZoom={false} style={{height: "100%", width: "100%", borderRadius: 50}}>
    <TileLayer
      attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    />
    <DraggableMarker attraction={attraction} onChangePosition={onChangePosition}/>
  </MapContainer>
}

export default InteractiveAttractionMap;