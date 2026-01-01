import { Injectable } from '@angular/core';
import {
  getAuth,
  signInWithEmailAndPassword,
  User
} from 'firebase/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private auth = getAuth();

  login(email: string, password: string) {
    return signInWithEmailAndPassword(this.auth, email, password);
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