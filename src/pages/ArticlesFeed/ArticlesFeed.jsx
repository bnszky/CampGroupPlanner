import "./ArticlesFeed.css"
import ArticleItem from "../../components/ArticleItem/ArticleItem.jsx"

export default function ArticlesFeed({articles}){
    return <>
        {articles.map(article => <ArticleItem key={article.id} article={article}/>)}
    </>
}