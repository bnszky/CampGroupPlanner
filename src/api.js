// api.js
export const fetchRegions = async () => {
    try {
        const response = await fetch(`/api/region/names`);
        if (!response.ok) {
            throw new Error('Failed to fetch regions');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(error.message);
    }
};
