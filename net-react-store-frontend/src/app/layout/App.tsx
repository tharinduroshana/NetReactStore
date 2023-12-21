import React, {useEffect, useState} from 'react';
import '../../App.css';
import {Product} from "../models/products";
import Catalog from "../../features/catelog/Catalog";

const App = () => {
    const [products, setProducts] = useState<Array<Product>>([]);

    useEffect(() => {
        fetchProducts();
    }, []);

    const fetchProducts = async () => {
        const response = await fetch("http://localhost:5198/api/products");
        const results = await response.json();
        setProducts(results);
    }

    return (
        <>
            <Catalog products={products} createProduct={() => console.log("test")} />
        </>
    );
}

export default App;
