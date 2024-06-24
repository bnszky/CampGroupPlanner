import React, { useState, useEffect } from 'react';
import {Slider, Box, Typography, Button} from '@mui/material';

export default function MinimalPositivitySlider() {

    const [minPositivityRate, setMinPositivityRate] = useState(0);
    const [initialRate, setInitialRate] = useState(0);
    const [showSubmitButton, setShowSubmitButton] = useState(false);

    useEffect(() => {
        const fetchInitialRate = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('/api/articles/get-min-rate', {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                    },
                });
                const data = await response.json();
                setMinPositivityRate(data);
                setInitialRate(data);
            } catch (error) {
                console.error('Error fetching initial rate:', error);
            }
        };

        fetchInitialRate();
    }, []);

    const handleSliderChange = (event, newValue) => {
        setMinPositivityRate(newValue);
        setShowSubmitButton(newValue !== initialRate);
    };

    const handleSubmit = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`/api/articles/set-min-rate/${minPositivityRate}`, {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });

            if (response.ok) {
                setInitialRate(minPositivityRate);
                setShowSubmitButton(false);
            } else {
                console.error('Error setting new rate');
            }
        } catch (error) {
            console.error('Error submitting new rate:', error);
        }
    };



  return (
    <Box sx={{width: 400}}>
        <Typography gutterBottom>
            Change minimal positivity rate
        </Typography>
        <Slider
            value={minPositivityRate}
            onChange={handleSliderChange}
            aria-labelledby="submit-slider"
            valueLabelDisplay="auto"
            step={1}
            marks
            min={0}
            max={100}
        />
        {showSubmitButton && (
            <Button
                color="primary"
                variant="contained"
                sx={{ mx: 1, my: 2 }}
                onClick={handleSubmit}
            >
                Submit Changes
            </Button>
        )}
    </Box>
  );
}