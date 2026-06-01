import { Link } from 'react-router-dom';
import BottomNav from './BottomNav.jsx';
import './HomeScreen.css';

function HomeScreen() {
  return (
    <div className="home-screen">
      {/* Top organic blob */}
      <svg
        className="blob blob-top"
        viewBox="0 0 390 360"
        preserveAspectRatio="none"
        xmlns="http://www.w3.org/2000/svg"
        aria-hidden="true"
      >
        <path
          d="M0,0 L390,0 L390,230
             C370,248 338,265 298,252
             C258,239 222,212 182,228
             C142,244 102,278 62,295
             C41,304 20,310 0,318
             Z"
          fill="#d4c5b3"
        />
      </svg>

      {/* Bottom organic blob */}
      <svg
        className="blob blob-bottom"
        viewBox="0 0 390 220"
        preserveAspectRatio="none"
        xmlns="http://www.w3.org/2000/svg"
        aria-hidden="true"
      >
        <path
          d="M0,220 L390,220 L390,130
             C362,118 328,132 295,118
             C262,104 232,82 198,96
             C164,110 128,136 94,126
             C60,116 30,96 0,108
             Z"
          fill="#d4c5b3"
        />
      </svg>

      {/* Animated marquee */}
      <Link to="/shop" className="marquee-link" aria-label="Enter shop">
        <div className="marquee-viewport">
          <div className="marquee-track">
            {Array.from({ length: 6 }).map((_, i) => (
              <span key={i} className="marquee-word">
                MAIA <span className="marquee-sep">·</span>
              </span>
            ))}
          </div>
          <div className="marquee-track" aria-hidden="true">
            {Array.from({ length: 6 }).map((_, i) => (
              <span key={i} className="marquee-word">
                MAIA <span className="marquee-sep">·</span>
              </span>
            ))}
          </div>
        </div>
      </Link>

      <BottomNav />
    </div>
  );
}

export default HomeScreen;
