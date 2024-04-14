import ArticlesFeed from '../pages/ArticlesFeed/ArticlesFeed'
import './App.css'

function App() {

  const articles = [
    {
      id: 1,
      title: "Beatiful World!",
      description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Maecenas placerat ac metus eget maximus. Suspendisse fermentum, eros a luctus laoreet, nunc nisl mattis turpis, quis tincidunt erat urna non lorem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Cras ornare lectus ac neque rhoncus, vitae consectetur arcu pretium. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.",
      author: "Somebody",
      createdAt: new Date("05-04-2020"),
      imgUrl: "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg",
      sourceLink: "https://www.niagarafallsstatepark.com/"
    },
    {
      id: 2,
      title: "Barcelona",
      description: "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!",
      author: "The Blonde Girl",
      createdAt: new Date("11-25-2022"),
      imgUrl: "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg",
      sourceLink: "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/"
    },
    {
      id: 3,
      title: "Rome",
      description: "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world, Rome has it all. Wander the cobblestone street with gelato in hand, spend some time people watching from the Spanish Steps, spend hours in the museums, and take in all of the stunning architecture the city has to offer. Rome is also a foodie's heaven, from the world-class cuisine to casual trattorias and takeaway pizzas. Here's my ultimate Rome travel guide!",
      author: "The Blonde Girl",
      createdAt: new Date("04-14-2024 15:41:00"),
      imgUrl: "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg",
      sourceLink: "https://www.theblondeabroad.com/ultimate-rome-travel-guide/"
    }
  ]

  return (
    <>
      <ArticlesFeed articles={articles}/>
    </>
  )
}

export default App
