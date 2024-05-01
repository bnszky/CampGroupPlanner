import * as React from 'react'
import { Marker, Popup } from 'react-leaflet'
import { Grid, Typography } from '@mui/material'
import { useState, useRef, useMemo, useEffect } from 'react'

function DraggableMarker({attraction, onChangePosition}) {
    const [position, setPosition] = useState([attraction.longitude, attraction.latitude])
    const [markerKey, setMarkerKey] = useState(Math.random())
    const markerRef = useRef(null)
    const eventHandlers = useMemo(
      () => ({
        dragend() {
          const marker = markerRef.current
          if (marker != null) {
            setPosition(marker.getLatLng())
            onChangePosition(marker.getLatLng());
          }
        },
      }),
      [],
    )

    useEffect(() => {
        setMarkerKey(Math.random());
    }, [attraction]);
  
    return (
      <Marker
        draggable
        eventHandlers={eventHandlers}
        position={position}
        ref={markerRef}>
        <Popup minWidth={90}>
          <span>
            <Grid align="center">
                <img src={attraction.image} alt={`image ${attraction.name}`} style={{width: 150, height: 100, margin: 0}}/>
                <Typography variant='body1' align='center'>{attraction.name}</Typography>
            </Grid>
          </span>
        </Popup>
      </Marker>
    )
  }

export default DraggableMarker