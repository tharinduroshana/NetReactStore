import {
  AppBar,
  Badge,
  Box,
  IconButton,
  List,
  ListItem,
  Switch,
  Toolbar,
  Typography,
} from "@mui/material";
import { Link, NavLink } from "react-router-dom";
import { ShoppingCart } from "@mui/icons-material";
import { useStoreContext } from "../context/StoreContext";

type HeaderProps = {
  darkMode: boolean;
  handleThemeChange: () => void;
};

const midLinks = [
  { title: "Catalog", path: "/catalog" },
  { title: "About", path: "/about" },
  { title: "Contact", path: "/contact" },
];

const rightLinks = [
  { title: "Login", path: "/login" },
  { title: "Register", path: "/register" },
];

const navStyles = {
  color: "inherit",
  typography: "h6",
  "&:hover": { color: "grey.500" },
  "&.active": { color: "text.secondary" },
  textDecoration: "none",
};

const Header = ({ darkMode, handleThemeChange }: HeaderProps) => {
  const { basket } = useStoreContext();
  // use javascript reduce to count items in the basket
  const itemCount = basket?.items.reduce((acc, item) => acc + item.quantity, 0);

  return (
    <AppBar position="static" sx={{ mb: 4 }}>
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Box display="flex" alignItems="center">
          <Typography variant="h6" component={NavLink} to="/" sx={navStyles}>
            Net React Store
          </Typography>
          <Switch checked={darkMode} onChange={handleThemeChange} />
        </Box>
        <List sx={{ display: "flex" }}>
          {midLinks.map(({ title, path }) => (
            <ListItem key={path} component={NavLink} to={path} sx={navStyles}>
              {title.toUpperCase()}
            </ListItem>
          ))}
        </List>
        <Box display="flex" alignItems="center">
          <IconButton
            component={Link}
            size="large"
            edge="start"
            color="inherit"
            to="/basket"
            sx={{ mr: 2 }}
          >
            <Badge badgeContent={itemCount} color="secondary">
              <ShoppingCart />
            </Badge>
          </IconButton>
          <List sx={{ display: "flex" }}>
            {rightLinks.map(({ title, path }) => (
              <ListItem key={path} component={NavLink} to={path} sx={navStyles}>
                {title.toUpperCase()}
              </ListItem>
            ))}
          </List>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
