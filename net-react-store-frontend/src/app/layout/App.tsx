import React, {useEffect, useState} from 'react';
import '../../App.css';
import {Product} from "../models/products";

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
        <div>
            <h1>Test</h1>
          <div>{products.map((item) => (<div key={item.id}>{item.name}</div>))}</div>
        </div>
    );
}

export default App;
