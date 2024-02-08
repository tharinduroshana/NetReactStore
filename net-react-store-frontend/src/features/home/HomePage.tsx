import Slider from "react-slick";
import { Box } from "@mui/material";
import Typography from "@mui/material/Typography";

const HomePage = () => {
  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
  };
  return (
    <>
      <Slider {...settings}>
        <img
          src="/images/iphone.png"
          alt="iphone"
          style={{ display: "block", width: "50%", maxHeight: 500 }}
        />
        <img
          src="/images/s23.jpeg"
          alt="samsung"
          style={{ display: "block", width: "50%", maxHeight: 500 }}
        />
      </Slider>
      <Box display="flex" justifyContent="center" sx={{ p: 4 }}>
        <Typography variant="h1">Welcome to the Net React Store</Typography>
      </Box>
    </>
  );
};

export default HomePage;
