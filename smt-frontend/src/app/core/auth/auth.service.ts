import { Injectable } from '@angular/core';
import {
    browserLocalPersistence,
  getAuth,
  setPersistence,
  signInWithEmailAndPassword,
  User
} from 'firebase/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private auth = getAuth();

   constructor() {
    // Persist user session across page reloads
    setPersistence(this.auth, browserLocalPersistence);
  }

    async login(email: string, password: string): Promise<void> {
    //Frontend validation
    if (!email || !password) {
      throw new Error('Email and password are required');
    }

    try {
      await setPersistence(this.auth, browserLocalPersistence);
      await signInWithEmailAndPassword(this.auth, email, password);
    } catch (error: any) {

    switch (error.code) {
      case 'auth/invalid-credential':
      case 'auth/wrong-password':
      case 'auth/user-not-found':
        throw new Error('Invalid email or password');

      case 'auth/invalid-email':
        throw new Error('Please enter a valid email address');

      case 'auth/too-many-requests':
        throw new Error('Too many login attempts. Please try again later.');

      default:
        throw new Error('Login failed. Please try again.');
    }
  }

  }

  logout() {
    return this.auth.signOut();
  }

  getCurrentUser(): User | null {
    return this.auth.currentUser;
  }

  async getIdToken(): Promise<string | null> {
    const user = this.auth.currentUser;
    if (!user) return null;
    return await user.getIdToken();
  }

  isLoggedIn(): boolean {
    return !!this.auth.currentUser;
  }
}