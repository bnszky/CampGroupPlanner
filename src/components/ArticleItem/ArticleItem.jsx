import HumanDate from "../HumanDate/HumanDate";
import "./ArticleItem.css" 
import PropTypes from 'prop-types';

import { Button } from "react-bootstrap";

function ArticleItem({article}) {
    return <div className="cart row">
        <img src={article.imgUrl} className="col-md-6" alt="Image"/>
        <div className="article-content col-md-6">
            <div className="title-section">
                <h1 className="title">{article.title}</h1>
                <div className="article-info">
                    <p className="author">Author: {article.author}</p>
                    <HumanDate date={article.createdAt}/>
                </div>
            </div>
            <p className="description">{article.description}</p>
            <Button as="a" variant="primary" href={article.sourceLink} target="_blank">Read more...</Button>
        </div>
    </div>
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