import * as React from 'react'
import { List, ListItem, ListItemButton, ListItemText, Typography } from '@mui/material';
import AttractionCard from '../AttractionCard/AttractionCard';

function AttractionsList({attractions, handleEdit, handleDelete}) {
    return <>
    <Typography variant='h4' align='center' p={2}>Attractions</Typography>
    <List sx={{
        maxHeight: '80%',
        overflow: 'auto',
        border: 2,
        borderColor: 'gray',
        padding: 2
    }}>
        {attractions.map(attraction => <AttractionCard key={attraction.id} attraction={attraction} handleDelete={() => handleDelete(attraction.id)} handleEdit={() => handleEdit(attraction.id)}/>)}
    </List>
    </>
}

export default AttractionsList;