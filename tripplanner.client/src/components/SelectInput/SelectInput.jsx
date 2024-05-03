import * as React from 'react'
import { InputLabel, Select, FormControl, Box, MenuItem, Typography } from '@mui/material';
import toCamelCase from '../../functions/toCamelCase';

function SelectInput({fieldName, onValueChange, items}) {

    const [val, setVal] = React.useState(items[0]);

    const handleChange = (event) => {
        setVal(event.target.value);
        onValueChange(event.target.value);
    };

    return <Box>
        <Typography variant='body1'>{toCamelCase(fieldName)}</Typography>
    <FormControl fullWidth>
      <InputLabel id={`${fieldName}-label`}>{toCamelCase(fieldName)}</InputLabel>
      <Select
        sx={{width: 400}}
        labelId={`${fieldName}-label`}
        id={fieldName}
        value={val}
        label={fieldName}
        onChange={handleChange}
      >
        {items.map(item => <MenuItem key={item} value={item}>{item}</MenuItem>)}
      </Select>
    </FormControl>
  </Box>
}

export default SelectInput;