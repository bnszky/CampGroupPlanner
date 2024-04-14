import HumanDate from "../HumanDate/HumanDate";
import "./ArticleItem.css" 
import PropTypes from 'prop-types';

function ArticleItem({article}) {
    return <div className="card">
        <img src={article.imgUrl} alt="Image"/>
        <div className="article-content">
            <div className="title-section">
                <h1 className="title">{article.title}</h1>
                <div className="article-info">
                    <p className="author">Author: {article.author}</p>
                    <HumanDate date={article.createdAt}/>
                </div>
            </div>
            <p className="description">{article.description}</p>
            <a className="link-btn" href={article.sourceLink} target="_blank">Read more</a>
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