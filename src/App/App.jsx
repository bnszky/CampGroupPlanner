import ArticlesFeed from '../pages/ArticlesFeed/ArticlesFeed'
import RegionPage from '../pages/RegionPage/RegionPage'
import Example from '../pages/RegionPage/RegionPage'
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
    }
  ]

  const comments = [
    {
      id: 1,
      author: {
        nick: "michali",
        email: "michal@gmail.com",
        profileImage: "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
      },
      createdAt: new Date("04-14-2024 15:41:00"), 
      rate: 4.5,
      text: "Beautiful place! I would like to be there again"
    },
    {
      id: 2,
      author: {
        nick: "gombalo",
        email: "gombalo@gmail.com",
        profileImage: "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
      },
      createdAt: new Date("07-18-2023 15:41:00"), 
      rate: 2,
      text: "I don't like spanish people, Ughh..."
    },
    {
      id: 3,
      author: {
        nick: "wealthyGuy",
        email: "wealthyGuy@gmail.com",
        profileImage: "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
      },
      createdAt: new Date("07-18-2023 15:41:00"), 
      rate: 5,
      text: "Very cheap region, I really recommend you to eat out"
    }
  ]

  return (
    <>
      <RegionPage region={regionCatalonia}/>
    </>
  )
}

export default App
