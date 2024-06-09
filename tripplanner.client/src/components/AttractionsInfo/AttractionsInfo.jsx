import { Typography, Grid } from "@mui/material";
import * as React from "react";
import AttractionsList from "../AttractionsList/AttractionsList";
import AttractionsMap from "../AttractionsMap/AttractionsMap";

function AttractionsInfo({attractions, handleDelete, handleEdit}) {
    return <Grid container spacing={7}>
        <Grid item xs={12} md={5} height={700}>
            <AttractionsList attractions={attractions} handleDelete={handleDelete} handleEdit={handleEdit}/>
        </Grid>
        <Grid item xs={12} md={7} height={700}>
            <AttractionsMap attractions={attractions}/>
        </Grid>
    </Grid>
}

export default AttractionsInfo;