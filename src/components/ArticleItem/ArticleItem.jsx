import HumanDate from "../HumanDate/HumanDate";
import "./ArticleItem.css" 
import PropTypes from 'prop-types';

import Button from "@mui/material/Button";
import { Card } from "@mui/material";

function ArticleItem({article}) {
    return <Card>
        
    </Card>
}

ArticleItem.propTypes = {
    article: PropTypes.shape({
        id: PropTypes.number,
        title: PropTypes.string,
        description: PropTypes.string,
        author: PropTypes.string,
        createdAt: PropTypes.object,
        imgUrl: PropTypes.string,
        sourceLink: PropTypes.string
    })
}

export default ArticleItem;