import { Typography, Grid } from "@mui/material";
import * as React from "react";
import { Container } from "react-bootstrap";
import AttractionsList from "../AttractionsList/AttractionsList";
import AttractionsMap from "../AttractionsMap/AttractionsMap";

function AttractionsInfo({attractions}) {
    return <Grid container spacing={7}>
        <Grid item xs={12} md={5} height={700}>
            <AttractionsList attractions={attractions}/>
        </Grid>
        <Grid item xs={12} md={7} height={700}>
            <AttractionsMap attractions={attractions}/>
        </Grid>
    </Grid>
}

export default AttractionsInfo;