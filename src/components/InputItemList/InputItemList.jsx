import { Stack, Chip, Typography, Fab, Box, TextField, IconButton} from '@mui/material';
import * as React from 'react'

function InputItemList({name, onItemListChange, children}) {

    const childrenWithProps = React.Children.map(children, child => {
        if (React.isValidElement(child)) {
            return React.cloneElement(child, { handleAdd: handleAdd });
        }
        return child;
    });

    const colors = ['primary', 'secondary', 'error', 'warning', 'info', 'success']

    function getRandomColor(){
        return colors[(Math.floor(Math.random() * colors.length))]
    }

    const [itemList, setItemList] = React.useState(new Set());
    const [colorsDict, setColorsDict] = React.useState({});

    function handleDelete(item){
        let _itemList = new Set([...itemList]);

        _itemList.delete(item);

        setItemList(_itemList);
        onItemListChange(_itemList);
    };

    function handleAdd(item){
        let _itemList = new Set([...itemList]);

        _itemList.add(item);

        let _colorsDict = {...colorsDict}

        _colorsDict[item] = getRandomColor();
        setColorsDict(_colorsDict);
        
        setItemList(_itemList);
        onItemListChange(_itemList);
    }

    return <Box>
        
        <Typography variant="body1">{name}</Typography>

        <Box sx={{width: 400}} direction='row'>
            {(itemList.size <= 0) && <Typography variant='body2' m={1}>No items</Typography>}
            {[...itemList].map((item, key) => (
                <Chip color={colorsDict[item]} sx={{margin: 1}} key={key} label={item} onDelete={() => handleDelete(item)}/>
            ))}
        </Box>

        {childrenWithProps}
    
    </Box>;
}

export default InputItemList;