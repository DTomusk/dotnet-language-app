import { createBrowserRouter } from 'react-router-dom'
import LoginPage from '../pages/LoginPage'
import RegistrationPage from '../pages/RegistrationPage'
import HomePage from '../pages/HomePage'

export const router = createBrowserRouter([
    {
        path: '/',
        element: <HomePage />   
    },
    {
        path: '/login',
        element: <LoginPage />
    },
    {
        path: '/register',
        element: <RegistrationPage />
    },
])