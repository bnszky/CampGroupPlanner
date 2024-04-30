function toCamelCase(text){
    return text.slice(0, 1).toUpperCase() + text.slice(1, text.length).toLowerCase()
}

export default toCamelCase;