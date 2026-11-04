import './App.css'
import StartScreen from './components/StartScreen'
import GameScreen from './components/GameScreen'
import ResultScreen from './components/ResultScreen'
import SummaryScreen from './components/SummaryScreen'
import { useGameSession } from './hooks/useGameSession'

function App() {
  const {
    screen,
    maps,
    round,
    roundIndex,
    totalRounds,
    isLastRound,
    totalScore,
    guess,
    result,
    summary,
    handleStart,
    handleGuess,
    handleNext,
    handlePlayAgain,
  } = useGameSession()

  return (
    <>
      {(screen === 'playing' || screen === 'result') && (
        <div className="game-progress">
          Round {roundIndex + 1} / {totalRounds} · Total: {totalScore}
        </div>
      )}
      {screen === 'start' && <StartScreen maps={maps} onStart={handleStart} />}
      {screen === 'playing' && round && <GameScreen key={round.locationId} round={round} onGuess={handleGuess} />}
      {screen === 'result' && round && guess && result && (
        <ResultScreen
          round={round}
          guess={guess}
          result={result}
          nextLabel={isLastRound ? 'See summary' : 'Next round'}
          onNext={handleNext}
        />
      )}
      {screen === 'summary' && summary && <SummaryScreen summary={summary} onPlayAgain={handlePlayAgain} />}
    </>
  )
}

export default App
