import { useEffect, useState } from 'react'
import { getMaps, getSummary, startGame, submitGuess } from '../api'
import type { GameDto, GameSummaryDto, GuessPoint, GuessResultDto, MapDto, RoundGuessDto } from '../types'

type Screen = 'start' | 'playing' | 'result' | 'summary'

/** Owns the state of one play-through (maps, rounds, guesses, score) and the transitions between screens. */
export function useGameSession() {
  const [screen, setScreen] = useState<Screen>('start')
  const [maps, setMaps] = useState<MapDto[]>([])
  const [game, setGame] = useState<GameDto | null>(null)
  const [roundIndex, setRoundIndex] = useState(0)
  const [guesses, setGuesses] = useState<RoundGuessDto[]>([])
  const [totalScore, setTotalScore] = useState(0)
  const [guess, setGuess] = useState<GuessPoint | null>(null)
  const [result, setResult] = useState<GuessResultDto | null>(null)
  const [summary, setSummary] = useState<GameSummaryDto | null>(null)

  useEffect(() => {
    getMaps().then(setMaps).catch(console.error)
  }, [])

  const round = game?.rounds[roundIndex] ?? null
  const totalRounds = game?.rounds.length ?? 0
  const isLastRound = roundIndex === totalRounds - 1

  const handleStart = (code?: string) => {
    startGame(code)
      .then((nextGame) => {
        setGame(nextGame)
        setRoundIndex(0)
        setGuesses([])
        setTotalScore(0)
        setGuess(null)
        setResult(null)
        setSummary(null)
        setScreen('playing')
      })
      .catch(console.error)
  }

  const handleGuess = (point: GuessPoint) => {
    if (!round) return
    setGuess(point)
    submitGuess(round.locationId, point.x, point.y)
      .then((nextResult) => {
        setResult(nextResult)
        setTotalScore((total) => total + nextResult.score)
        setGuesses((previous) => [...previous, { locationId: round.locationId, x: point.x, y: point.y }])
        setScreen('result')
      })
      .catch(console.error)
  }

  const handleNext = () => {
    if (!isLastRound) {
      setRoundIndex((index) => index + 1)
      setGuess(null)
      setResult(null)
      setScreen('playing')
      return
    }
    getSummary(guesses)
      .then((nextSummary) => {
        setSummary(nextSummary)
        setScreen('summary')
      })
      .catch(console.error)
  }

  const handlePlayAgain = () => setScreen('start')

  return {
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
  }
}
