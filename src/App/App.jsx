import Footer from '../components/Footer/Footer'
import Navbar from '../components/Navbar/Navbar'
import ArticlesFeed from '../pages/ArticlesFeed/ArticlesFeed'
import CreateArticle from '../pages/CreateArticle/CreateArticle'
import CreateAttraction from '../pages/CreateAttraction/CreateAttraction'
import RegionPage from '../pages/RegionPage/RegionPage'
import Example from '../pages/RegionPage/RegionPage'
import './App.css'
import { Box } from '@mui/material'

function App() {

  const articles = [
    {
      id: 1,
      title: "Beatiful World!",
      description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Maecenas placerat ac metus eget maximus. Suspendisse fermentum, eros a luctus laoreet, nunc nisl mattis turpis, quis tincidunt erat urna non lorem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Cras ornare lectus ac neque rhoncus, vitae consectetur arcu pretium. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.",
      createdAt: new Date("05-04-2020"),
      imgUrl: "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg",
      sourceLink: "https://www.niagarafallsstatepark.com/"
    },
    {
      id: 2,
      title: "Barcelona",
      description: "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!",
      createdAt: new Date("11-25-2022"),
      imgUrl: "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg",
      sourceLink: "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/"
    },
    {
      id: 3,
      title: "Rome",
      description: "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world, Rome has it all. Wander the cobblestone street with gelato in hand, spend some time people watching from the Spanish Steps, spend hours in the museums, and take in all of the stunning architecture the city has to offer. Rome is also a foodie's heaven, from the world-class cuisine to casual trattorias and takeaway pizzas. Here's my ultimate Rome travel guide!",
      createdAt: new Date("04-14-2024 15:41:00"),
      imgUrl: "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg",
      sourceLink: "https://www.theblondeabroad.com/ultimate-rome-travel-guide/"
    }
  ]

  const regionCatalonia = {
    name: "Catalonia",
    country: "Spain",
    cities: ["Barcelona", "Girona", "Tarragona"],
    description: "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!",
    images: [
      "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRnwf8dsSKIsCsVbwXlpQEuvEP6q70MdNVjdQ&s",
      "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRaRfTP8AW7Od72m4IRi4LPRt9xNqPYfYlPrg&s",
      "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS88opuDZn5DfqGYSFvihQ2RMO8PTub3Op-QA&s",
      "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRVFoCl2kKjoNsb-hi4S8Imbh0qdO-d4pqrVQ&s"
    ]
  }

  const attractions = [
    {
      id: 1,
      name: "Basílica de la Sagrada Familia",
      description: "Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city",
      image: "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/08/10/a7/d6/basilica-de-la-sagrada.jpg?w=1200&h=-1&s=1",
      longitude: 41.40377892106611,
      latitude: 2.174366495306742,
    },
    {
      id: 2,
      name: "Parc Guell",
      description: "Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city",
      image: "https://lh5.googleusercontent.com/p/AF1QipNgwQHFyIjmdNz9RYHLND4_2hXzrBmqObHjBIfR=w408-h305-k-no",
      longitude: 41.4146798295698,
      latitude: 2.152780327073272,
    },
    {
      id: 3,
      name: "Casa Batlló",
      description: "Welcome to Barcelona's magical house. A Gaudí masterpiece. A unique immersive experience. International Exhibition of the Year 2022. Children free up to 12 years old.",
      image: "https://dynamic-media-cdn.tripadvisor.com/media/daodao/photo-o/19/ac/b2/a5/caption.jpg?w=1200&h=-1&s=1",
      longitude: 41.39187830789514,
      latitude: 2.164871022478367,
    },
  ]

  const reviews = [
    {
      id: 1,
      author: {
        nick: "michali",
        profileImage: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGDohX4qAelLzi3t8vCfqccDFxifY-huxkmRrgnSRoig&s"
      },
      createdAt: new Date("04-14-2024 15:41:00"), 
      rate: 4.5,
      title: "Opinion 1",
      text: "Beautiful place! I would like to be there again"
    },
    {
      id: 2,
      author: {
        nick: "gombalo",
        profileImage: "https://static.vecteezy.com/system/resources/thumbnails/002/002/403/small/man-with-beard-avatar-character-isolated-icon-free-vector.jpg"
      },
      createdAt: new Date("07-18-2023 15:41:00"), 
      rate: 2,
      title: "Opinion 2",
      text: "I don't like spanish people, Ughh..."
    },
    {
      id: 3,
      author: {
        nick: "wealthyGuy",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("07-18-2023 15:41:00"), 
      rate: 5,
      title: "Opinion 3",
      text: "Very cheap region, I really recommend you to eat out"
    },
    {
      id: 4,
      author: {
        nick: "somebody",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("01-25-2024 15:41:00"), 
      rate: 1,
      title: "Opinion 4",
      text: "Hello! lorelorleroleolroelrlelrelo"
    },
    {
      id: 5,
      author: {
        nick: "Amanda",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("07-18-2020 15:41:00"), 
      rate: 3.5,
      title: "Opinion 5",
      text: "I'd say that's average"
    },
    {
      id: 6,
      author: {
        nick: "impolite Man",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("07-18-2022 15:41:00"), 
      rate: 4.5,
      title: "Opinion 6",
      text: "Everything's good but you have too many 5 stars so that's why I give you 4 and half"
    }
  ]

  const regions = ['None', 'Catalonia', 'Lombardia', 'Venezia', 'Bavaria'];

  return (
    <>
      <Navbar/>
      <Box sx={{
        padding: '2rem',
        paddingBottom: '5rem'
      }}>

      {/*<RegionPage attractions={attractions} articles={articles} reviews={reviews} region={regionCatalonia}/>*/}
      {/*<CreateArticle regions={regions}/>*/}
      {<CreateAttraction regions={regions}/>}

      </Box>
      <Footer/>
    </>
  )
}

export default App
